using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rottytooth.Esolang.Velato.Tokens;

namespace Rottytooth.Esolang.Velato
{
    public class Parser
    {
        public const int MINOR_SECOND = 1;
        public const int MAJOR_SECOND = 2;
        public const int MINOR_THIRD = 3;
        public const int MAJOR_THIRD = 4;
        public const int PERFECT_FOURTH = 5;
        public const int DIMINISHED_FIFTH = 6;
        public const int PERFECT_FIFTH = 7;
        public const int MINOR_SIXTH = 8;
        public const int MAJOR_SIXTH = 9;
        public const int MINOR_SEVENTH = 10;
        public const int MAJOR_SEVENTH = 11;


        public List<Note> Notes { get; private set; }
        public int BaseInterval { get; private set; }

        private List<CommandToken> _tokens { get; set; }
        private Note _baseNote { get; set; }

        public Parser(MidiLoader loader)
        {
            this.Notes = loader.Notes;
            this.BaseInterval = loader.SmallestInterval;
        }

        public Parser(List<Note> notes, int baseInterval)
        {
            this.Notes = notes;
            this.BaseInterval = baseInterval;
        }

        public List<CommandToken> Parse()
        {
            _baseNote = null;
            _tokens = new List<CommandToken>();

            for(int index = 0; index < Notes.Count; )
            {
                Notes[index].PitchCorrectionUnits =
                    (int)Math.Round(Notes[index].PitchCorrection * BaseInterval);

                if (_baseNote == null)
                {
                    _baseNote = Notes[index];
                    index++;
                    continue;
                }
                CommandToken commandToken = ParseCommand(ref index);
                if (commandToken != null)
                {
                    _tokens.Add(commandToken);
                }
            }

            return _tokens;
        }

        private int GetInterval(int index)
        {
            Note note = Notes[index];
            double rootNumber = _baseNote.Number;
            while (rootNumber > note.Number)
            {
                rootNumber -= 12;
            }
            double interval = note.Number - rootNumber;
            double pitchCorrectedInterval =
                (note.Number * this.BaseInterval / 2.0 + note.PitchCorrectionUnits) -
                (rootNumber * this.BaseInterval / 2.0 + _baseNote.PitchCorrectionUnits);
            int commandInterval = 0;
            try
            {
                commandInterval = Convert.ToInt32(pitchCorrectedInterval);
            }
            catch (FormatException ex)
            {
                throw new CompilerException("Could not parse command: non-integer interval at note #" + index.ToString());
            }

            return commandInterval % 12;
        }

        public static string GetIntervalName(int interval)
        {
            interval = interval % 12; // just in case it has not already been modded

            switch(interval)
            {
                case 0:
                    return "Octave or Unison";
                case MINOR_SECOND:
                    return "Minor Second";
                case MAJOR_SECOND:
                    return "Major Second";
                case MINOR_THIRD:
                    return "Minor Third";
                case MAJOR_THIRD:
                    return "Major Third";
                case PERFECT_FOURTH:
                    return "Perfect Fourth";
                case DIMINISHED_FIFTH:
                    return "Diminished Fifth";
                case PERFECT_FIFTH:
                    return "Perfect Fifth";
                case MINOR_SIXTH:
                    return "Minor Sixth";
                case MAJOR_SIXTH:
                    return "Major Sixth";
                case MINOR_SEVENTH:
                    return "Minor Seventh";
                case MAJOR_SEVENTH:
                    return "Major Seventh";
            }
            throw new CompilerException("Could not determine interval size");
        }

        private CommandToken ParseCommand(ref int index)
        {
            int commandInterval = GetInterval(index);

            switch (commandInterval)
            {
                case 0:
                    // this is not a command at all, just a holding place
                    index++;
                    return null;
                case MAJOR_SECOND:
                    // new root note
                    // get next interval and that is the new root note
                    index++; // get next index
                    _baseNote = Notes[index];
                    return null;
                case MINOR_THIRD:
                    // let (variable as single note, then expression)
                    index++; // get next index
                    CommandToken letCommand = new CommandToken()
                    {
                        CommandType = CommandType.Let,
                        VariableName = Notes[index]
                    };
                    index++;
                    letCommand.ChildExpressions.Add(ParseExpression(ref index));
                    return letCommand;
                case MAJOR_THIRD:
                    // this indicates a block, need next note to determine type
                    index++;
                    ParseBlock(ref index);

                    return null;
                case MINOR_SIXTH: // minor sixth
                    // declare
                    CommandToken declareCommand = new CommandToken()
                    {
                        CommandType = CommandType.Declare,
                        VariableName = Notes[index + 1],
                        Type = ParseType(index + 2)
                    };
                    index += 3; // get next index
                    return declareCommand;
                case MAJOR_SIXTH: // major sixth^
                    index++;
                    return ParseSpecialCommand(ref index);
                default:
                    throw new SyntaxError("Invalid first interval of command", commandInterval, index, Notes[index].Name);
            }
        }

        private CommandToken ParseBlock(ref int index)
        {
            int commandInterval = GetInterval(index);
            CommandToken subToken = null;
            List<CommandToken> subCommands = null;

            switch (commandInterval)
            {
                case MAJOR_THIRD:
                    // while
                    subCommands = new List<CommandToken>();
                    do
                    {
                        index++;
                        subToken = ParseCommand(ref index);
                        subCommands.Add(subToken);
                    } while (subToken == null || subToken.CommandType != CommandType.EndWhile);
                    return new CommandToken()
                    {
                        CommandType = CommandType.Else,
                        ChildCommands = subCommands
                    };
                case PERFECT_FOURTH:
                    // end while
                    return new CommandToken()
                    {
                        CommandType = CommandType.EndWhile
                    };
                case PERFECT_FIFTH:
                    // if
                    subCommands = new List<CommandToken>();
                    do
                    {
                        index++;
                        subToken = ParseCommand(ref index);
                        subCommands.Add(subToken);
                    } while (subToken == null || subToken.CommandType != CommandType.EndIf ||
                        subToken.CommandType != CommandType.Else); // else will hold its own commands
                    return new CommandToken()
                    {
                        CommandType = CommandType.Else,
                        ChildCommands = subCommands
                    };
                case MAJOR_SIXTH:
                    // else
                    subCommands = new List<CommandToken>();
                    do
                    {
                        index++;
                        subToken = ParseCommand(ref index);
                        subCommands.Add(subToken);
                    } while (subToken == null || subToken.CommandType != CommandType.EndIf);

                    return new CommandToken()
                    {
                        CommandType = CommandType.Else,
                        ChildCommands = subCommands
                    };
                case MAJOR_SEVENTH:
                    // end if
                    return new CommandToken()
                    {
                        CommandType = CommandType.EndIf
                    };
                default:
                    throw new SyntaxError("Could not determine block command with interval", commandInterval, index);
            }
        }

        private CommandToken ParseSpecialCommand(ref int index)
        {
            int commandInterval = GetInterval(index);

            switch (commandInterval % 12)
            {
                case PERFECT_FOURTH:
                case MAJOR_SIXTH:
                    // input command
                    index++; // get next index
                    CommandToken inputCommand = new CommandToken()
                    {
                        CommandType = CommandType.Input,
                        VariableName = Notes[index]
                    };
                    index++;
                    return inputCommand;
                case PERFECT_FIFTH:
                    // print command
                    CommandToken printCommand = new CommandToken()
                    {
                        CommandType = CommandType.Print
                    };
                    index++;
                    printCommand.ChildExpressions.Add(ParseExpression(ref index));
                    return printCommand;
                default:
                    throw new SyntaxError("Could not determine special command with interval ", commandInterval, index);
            }
        }

        // while multi-interval commands are broken down into different functions, all the expressions are interpreted
        // in this one method, to avoid sprawling methods
        private ExpressionToken ParseExpression(ref int index)
        {
            int expressionInterval = GetInterval(index);
            int secondInterval, thirdInterval;
            int intValue;
            double doubleValue;

            switch (expressionInterval % 12)
            {
                case MINOR_SECOND:
                case MAJOR_SECOND:
                    // conditionals
                    index++;
                    secondInterval = GetInterval(index);
                    switch(secondInterval)
                    {
                        case MINOR_SECOND:
                        case MAJOR_SECOND:
                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.Equal
                            };
                        case MINOR_THIRD:
                        case MAJOR_THIRD:
                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.GreaterThan
                            };
                        case PERFECT_FOURTH:
                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.LessThan
                            };
                        case DIMINISHED_FIFTH:
                        case PERFECT_FIFTH:
                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.Not
                            };
                        case MINOR_SIXTH:
                        case MAJOR_SIXTH:
                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.And
                            };
                        case MINOR_SEVENTH:
                        case MAJOR_SEVENTH:
                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.Or
                            };
                        default:
                            throw new SyntaxError("Could not determine expression with " +
                                GetIntervalName(expressionInterval) + " followed by", secondInterval, index);
                    }
                case MINOR_THIRD:
                case MAJOR_THIRD:
                    // value
                    index++;
                    secondInterval = GetInterval(index);
                    switch(secondInterval)
                    {
                        case MINOR_SECOND:
                        case MAJOR_SECOND:
                            // name of variable (single note)
                            index++;
                            Note variableName = Notes[index];
                            index++;
                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.Variable,
                                VariableName = variableName
                            };
                        case MINOR_THIRD:
                        case MAJOR_THIRD:
                            // neg. (-) int
                            // Single note for each digit, ending with Perfect 5th to mark end of number
                            index++;
                            if (!Int32.TryParse(GetDigits(ref index), out intValue))
                            {
                                throw new CompilerException("Could not determine int value" + intValue.ToString() +
                                    ", at note#" + index.ToString());
                            }

                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.Value,
                                Type = Tokens.Type.Int,
                                IntValue = 0 - intValue
                            };

                        case PERFECT_FOURTH:
                            // char
                            // Char as ASCII int: single note for each digit, ending with Perfect 5th to mark end of number
                            index++;
                            if (!Int32.TryParse(GetDigits(ref index), out intValue))
                            {
                                throw new CompilerException("Could not determine char value, at note#" + index.ToString());
                            }

                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.Value,
                                Type = Tokens.Type.Char,
                                IntValue = intValue,
                                CharValue = (char)intValue
                            };

                        case DIMINISHED_FIFTH:
                        case PERFECT_FIFTH:
                            // pos. (+) int
                            // Single note for each digit, ending with Perfect 5th to mark end of number
                            index++;

                            if (!Int32.TryParse(GetDigits(ref index), out intValue))
                            {
                                throw new CompilerException("Could not determine int value, at note#" + index.ToString());
                            }

                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.Value,
                                Type = Tokens.Type.Int,
                                IntValue = intValue
                            };

                        case MINOR_SIXTH:
                        case MAJOR_SIXTH:
                            // pos. (+) double
                            // Single note for each digit, first Perfect 5th to mark decimal point, second Perfect 5th marking end of number
                            index++;
                            if (!Double.TryParse(GetDigits(ref index) + "." + GetDigits(ref index), out doubleValue))
                            {
                                throw new CompilerException("Could not determine double value, at note#" + index.ToString());
                            }

                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.Value,
                                Type = Tokens.Type.Double,
                                DoubleValue = doubleValue
                            };

                        case MINOR_SEVENTH:
                        case MAJOR_SEVENTH:
                            // neg. (-) double
                            // Single note for each digit, first Perfect 5th to mark decimal point, second Perfect 5th marking end of number
                            index++;
                            if (!Double.TryParse(GetDigits(ref index) + "." + GetDigits(ref index), out doubleValue))
                            {
                                throw new CompilerException("Could not determine double value, at note#" + index.ToString());
                            }

                            return new ExpressionToken()
                            {
                                ExpressionType = ExpressionType.Value,
                                Type = Tokens.Type.Double,
                                DoubleValue = 0 - doubleValue
                            };

                        default:
                            throw new SyntaxError("Could not determine expression with " +
                                GetIntervalName(expressionInterval) + " followed by", secondInterval, index);
                    }

                case PERFECT_FIFTH:
                    // math operations
                    index++;
                    secondInterval = GetInterval(index);
                    switch (secondInterval)
                    {
                        case PERFECT_FIFTH:
                            // simple math operations
                            index++;
                            thirdInterval = GetInterval(index);
                            switch (thirdInterval)
                            {
                                case MINOR_SECOND:
                                case MAJOR_SECOND:
                                    return new ExpressionToken()
                                    {
                                        ExpressionType = ExpressionType.Minus
                                    };
                                case MINOR_THIRD:
                                case MAJOR_THIRD:
                                    return new ExpressionToken()
                                    {
                                        ExpressionType = ExpressionType.Plus
                                    };
                                case PERFECT_FOURTH:
                                    return new ExpressionToken()
                                    {
                                        ExpressionType = ExpressionType.Divide
                                    };
                                case DIMINISHED_FIFTH:
                                case PERFECT_FIFTH:
                                    return new ExpressionToken()
                                    {
                                        ExpressionType = ExpressionType.Multiply
                                    };
                                case MINOR_SIXTH:
                                case MAJOR_SIXTH:
                                    return new ExpressionToken()
                                    {
                                        ExpressionType = ExpressionType.Mod
                                    };
                                default:
                                    throw new SyntaxError("Could not determine expression with " +
                                        GetIntervalName(expressionInterval) + " followed by " +
                                        GetIntervalName(secondInterval), thirdInterval, index);
                            }

                        default:
                            throw new SyntaxError("Could not determine expression with " +
                                GetIntervalName(expressionInterval) + " followed by", secondInterval, index);
                    }
                case MINOR_SIXTH:
                case MAJOR_SIXTH:
                    // procedural
                    index++;
                    secondInterval = GetInterval(index);
                    switch (secondInterval)
                    {
                        case MINOR_SIXTH:
                        case MAJOR_SIXTH:
                            index++;
                            thirdInterval = GetInterval(index);
                            switch(thirdInterval)
                            {
                                case MINOR_SECOND:
                                case MAJOR_SECOND:
                                    return new ExpressionToken()
                                    {
                                        ExpressionType = ExpressionType.CloseParanthesis
                                    };
                                case MINOR_SIXTH:
                                case MAJOR_SIXTH:
                                    return new ExpressionToken()
                                    {
                                        ExpressionType = ExpressionType.OpenParanthesis
                                    };
                                default:
                                    throw new SyntaxError("Could not determine expression with " +
                                        GetIntervalName(expressionInterval) + " followed by " + 
                                        GetIntervalName(secondInterval), thirdInterval, index);
                            }
                        default:
                            throw new SyntaxError("Could not determine expression with " +
                                GetIntervalName(expressionInterval) + " followed by", secondInterval, index);
                    }
                default:
                    throw new SyntaxError("Could not determine special command with interval", expressionInterval, index);
            }
        }

        private string GetDigits(ref int index)
        {
            StringBuilder retDigits = new StringBuilder();
            int currentInterval = 0;
            for(; currentInterval != PERFECT_FIFTH; index++)
            {
                currentInterval = GetInterval(index);
                if (currentInterval % 12 < PERFECT_FIFTH)
                {
                    // Starts with flat second
                    retDigits.Append((currentInterval - 1).ToString());
                }
                else if (currentInterval % 12 > PERFECT_FIFTH)
                {
                    retDigits.Append((currentInterval - 2).ToString());
                }
            }

            return retDigits.ToString();
        }

        private Tokens.Type ParseType(int index)
        {
            int typeInterval = GetInterval(index);

            switch (typeInterval % 12)
            {
                case MINOR_SECOND:
                case MAJOR_SECOND:
                    return Tokens.Type.Int;
                case MINOR_THIRD:
                case MAJOR_THIRD:
                    return Tokens.Type.Char;
                case PERFECT_FOURTH:
                    return Tokens.Type.Double;
                default:
                    throw new SyntaxError("Could not determine type with interval", typeInterval, index);
            }
        }
    }
}


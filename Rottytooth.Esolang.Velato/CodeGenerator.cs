using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using Rottytooth.Esolang.Velato.Tokens;

namespace Rottytooth.Esolang.Velato
{
    public class CodeGenerator
    {
        public List<CommandToken> Tokens { get; private set; }

        public string ProgramName { get; private set; }

        private string CSharp { get; set; }

        public CodeGenerator(List<CommandToken> tokens, string programName)
        {
            this.Tokens = tokens;
            this.ProgramName = programName;
            this.CSharp = "";
        }

        public bool GenerateFile()
        {
            string errors = "";
            bool succeeded = Compile(ref errors);

            if (succeeded)
            {
                Console.WriteLine("Compiled Successfully to " + this.ProgramName + ".exe");
                return true;
            }
            else
            {
                Console.Error.Write("Could not compile.\nERRORS:\n\n");
                Console.Error.WriteLine(errors);
                return false;
            }
        }

        public string GenerateCSharp()
        {
            if (CSharp != "") return CSharp; // avoid generating more than once

            StringBuilder program = new StringBuilder();

            UnpackCommands(Tokens, program, false);

            CSharp = program.ToString();

            return program.ToString();
        }

        public string GenerateJavaScript()
        {
            StringBuilder program = new StringBuilder();

            UnpackCommands(Tokens, program, true);

            return program.ToString();
        }

        private void UnpackCommands(List<CommandToken> commands, StringBuilder program, bool js)
        {
            foreach (CommandToken token in Tokens)
            {
                switch (token.CommandType)
                {
                    case CommandType.Declare:
                        if (token.VariableName == null)
                        {
                            throw new CompilerException("Declare command with no varaible name");
                        }
                        string variableName = token.VariableName.Name + token.VariableName.Number;

                        if (js)
                        {
                            program.AppendLine("var " + variableName + ";");
                            break;
                        }

                        switch (token.Type)
                        {
                            case Velato.Tokens.Type.Int:
                                program.AppendLine("int " + variableName + ";");
                                break;
                            case Velato.Tokens.Type.Char:
                                program.AppendLine("char " + variableName + ";");
                                break;
                            case Velato.Tokens.Type.Double:
                                program.AppendLine("double " + variableName + ";");
                                break;
                        }
                        break;

                    case CommandType.Else:
                        program.AppendLine("} else {");
                        break;
                    case CommandType.EndIf:
                    case CommandType.EndWhile:
                        program.AppendLine("}");
                        break;
                    case CommandType.If:
                        program.Append("if (");
                        UnpackExpressions(token.ChildExpressions, program, js);
                        program.AppendLine(") {");
                        UnpackCommands(token.ChildCommands, program, js);
                        program.AppendLine("}");
                        break;
                    case CommandType.Let:
                        if (token.VariableName == null)
                        {
                            throw new CompilerException("Declare command with no varaible name");
                        }
                        string letVariableName = token.VariableName.Name + token.VariableName.Number;
                        program.Append(letVariableName + " = ");
                        UnpackExpressions(token.ChildExpressions, program, js);
                        program.AppendLine(";");
                        break;
                    case CommandType.Print:
                        if (js)
                        {
                            program.Append("document.getElementById('output').innerHTML += ");
                        }
                        else
                        {
                            program.Append("Console.Write(");
                        }
                        UnpackExpressions(token.ChildExpressions, program, js);
                        if (js)
                        {
                            program.AppendLine(";");
                        }
                        else
                        {
                            program.AppendLine(");");
                        }
                        break;
                    case CommandType.While:
                        program.Append("while (");
                        UnpackExpressions(token.ChildExpressions, program, js);
                        program.AppendLine(") {");
                        UnpackCommands(token.ChildCommands, program, js);
                        program.AppendLine("}");
                        break;
                }
            }
        }

        private void UnpackExpressions(List<ExpressionToken> expressions, StringBuilder program, bool js)
        {
            foreach(ExpressionToken token in expressions)
            {
                switch(token.ExpressionType)
                {
                    case ExpressionType.And:
                        program.Append(" && ");
                        break;
                    case ExpressionType.CloseParanthesis:
                        program.Append(")");
                        break;
                    case ExpressionType.Divide:
                        program.Append("/");
                        break;
                    case ExpressionType.Equal:
                        program.Append(" == ");
                        break;
                    case ExpressionType.GreaterThan:
                        program.Append(" > ");
                        break;
                    case ExpressionType.LessThan:
                        program.Append(" < ");
                        break;

                    // currently unavailable
                    //case ExpressionType.Log:
                    //case ExpressionType.Power:

                    case ExpressionType.Minus:
                        program.Append(" - ");
                        break;
                    case ExpressionType.Mod:
                        program.Append(" % ");
                        break;
                    case ExpressionType.Multiply:
                        program.Append(" * ");
                        break;
                    case ExpressionType.Not:
                        program.Append(" ! ");
                        break;
                    case ExpressionType.OpenParanthesis:
                        program.Append("(");
                        break;
                    case ExpressionType.Or:
                        program.Append(" || ");
                        break;
                    case ExpressionType.Plus:
                        program.Append(" + ");
                        break;
                    case ExpressionType.Value:
                        switch(token.Type)
                        {
                            case Velato.Tokens.Type.Char:
                                program.Append("'" + token.CharValue + "'");
                                break;
                            case Velato.Tokens.Type.Double:
                                program.Append(token.DoubleValue);
                                break;
                            case Velato.Tokens.Type.Int:
                                program.Append(token.IntValue);
                                break;
                        }
                        break;
                    case ExpressionType.Variable:
                        if (token.VariableName == null)
                        {
                            throw new CompilerException("Variable used with no varaible name");
                        }
                        string variableName = token.VariableName.Name + token.VariableName.Number;
                        program.Append(" " + variableName + " ");
                        break;
                }
            }
        }

        public bool Compile(ref string errors)
        {
            ;
            CSharpCodeProvider csc =
                new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" },
                this.ProgramName, true);
            parameters.GenerateExecutable = true;

            string entireProgram =
                @"using System;
                using System.IO;
                namespace Rottytooth.Esolang.Velato.Executable
                {
                    public static class Program {
                      public static void Main(string[] args) {
                        " + GenerateCSharp() + @"
                      }
                    }
                }";

            CompilerResults results = csc.CompileAssemblyFromSource(parameters, entireProgram);

            // output any errors
            StringBuilder errorList = new StringBuilder();

            results.Errors.Cast<CompilerError>().ToList().ForEach(error => errorList.AppendLine(error.ErrorText));

            if (errorList.Length > 0)
            {
                errors = errorList.ToString();
                return false;
            }

            // we have successfully compiled
            return true;
        }

    }
}


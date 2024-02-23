# Velato

Velato is a language where one writes code with music, in the form of a MIDI file. Read more on esolangs: http://esolangs.org/wiki/Velato

Current version: **[2.1](https://github.com/rottytooth/Velato/releases/tag/2.1.0)**

### Compiler Options
Code | Option
---|---
/s|transpile to C#
/j|transpile to JavaScript
/c|also compile (automatic when neither /s or /j is selected)

### Version 2.0 Changes
Version 2 of Velato was re-written from the ground up, using NAudio and a saner parsing strategy. Significant changes:

- Supports micro-tuning variants of the language
- Can transpile to C# or JavaScript code
- Looks at the first MIDI track, rather than the first channel (more in line with how MIDI is used)
- Examples are in the (more widely used) Lilypond, rather than the GUIDO system

\version "2.16.0"  % necessary for upgrading to future LilyPond versions.

\header{
  title = "Print 'He'"
  subtitle = "A test program for Velato"
}

mus = { f'4 % sets root note
	d c % print
	a bes % value -> char
	d gis c % 'H': digits 7 and 2 (for Unicode value 72) ending with a perfect 5th
	    
	d c a bes g fis g c} % same sequence but for the 'e' (101)

\score { 
        \new Staff \with { \remove Time_signature_engraver } { \clef bass  \mus } 
        \layout { } 
        \midi { } 
} 


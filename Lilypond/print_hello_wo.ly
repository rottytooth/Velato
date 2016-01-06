\version "2.16.0"  % necessary for upgrading to future LilyPond versions.

\header{
  title = "Print an e"
  subtitle = "A test program for Velato"
}

mus = { f'4 % sets root note
	d c % print
	a bes % value -> char
	cis g c % 'H': digits 7 and 2 (for Unicode value 72) ending with a perfect 5th
	    
	d c a bes fis f fis c

	f''

	f g c % change key to c

	a g e f cis c a g % l

	a g e f cis c a g % l

	c d f

	d c a bes fis fis fis c % o

	d c a bes a a c % ,

	d c a bes gis g c % space

	d c a bes d cis c % W

	f g d % change key to d

	d d'

	b a fis g dis dis dis a % o

} % same sequence but for the 'e' (101)

\score { 
        \new Staff \with { \remove Time_signature_engraver } { \clef bass  \mus } 
        \layout { } 
        \midi { } 
} 


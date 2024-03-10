\version "2.16.0"  % necessary for upgrading to future LilyPond versions.

\header{
  title = "Hello, World! (With Root Note Changes)"
  subtitle = "A test program for Velato"
}

mus = { 

	f'4 % sets root note
	d c % print
	a bes % value -> char
	d gis c % 'H': digits 7 and 2 (for Unicode value 72) ending with a perfect 5th
	    
	d c a bes % print
	g fis g c % 'e'

	f''

	f g c % change key to c

	a g e f d cis ais g % l

	a g e f d cis ais g % l

	c d f

	d c a bes g g g c % o

	d c a bes ais ais c % ,

	d c a bes a gis c % space

	d c a bes dis d c % W

	f g d % change key to d

	d d'

	b a fis g e e e a % o

	b a fis g e e g a % r

	b a fis g e dis c a % l

	e f

	d c a bes g fis fis c % d

	d c a bes a a c % !

} % same sequence but for the 'e' (101)

\score { 
        \new Staff \with { \remove Time_signature_engraver } { \clef bass  \mus } 
        \layout { } 
        \midi { } 
} 


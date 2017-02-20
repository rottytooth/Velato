\version "2.16.0"  % necessary for upgrading to future LilyPond versions.

\header{
  title = "Cat"
  subtitle = "A Velato program that copies input to output"
}

maintrack = { 
	\key f \major
	\time 4/4 
	c'
	aes'
	e''
	e' 
	e''
	a'
	f'
	e''
	a'
	g'
	ees' % or e'
	des' % or d'
	e''
	e'
	f' 2~
	f' 1
}

harmonize = {
	\clef bass
	\key f \major
	\time 4/4 
	<c, g, e> 4
	<c, aes, ees>
	<e, g, e>
	<e, a, d> 
	<e, a, c>
	<e, a, c>
	<c, g, c>
	<e, a, c>
	<f, a, a>
	<g, d, g>
	<g, c, bes>
	<f des, g>
	<f, c, f>
	<f, c, f>
	<g, c, e> 
	<g, bes,, e>
	<c, c,, a>1
}

\score { 
		\new StaffGroup <<
			\new Staff { \maintrack }
			\new Staff { \harmonize }
		>>
        \layout { } 
        \midi { } 
} 


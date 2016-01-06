\version "2.16.0"  % necessary for upgrading to future LilyPond versions.

\header{
  title = "Hello, World"
  subtitle = "A test program for Velato"
}

mus = { 
	\time 2/4
	<f d' c'' a'''>4
	bes8 cis8 

	<g c' d''>4
 
	<c a'>8 bes'8 
	fis'8 f'16 fis'16 
	<c' f'>8
	<f g'>16 <c a'>16 
	<g, e f' cis'>4

	<c, a g'>4

	<a, g e' f''>4 
	cis''8 c''8

	<a g' c''>8 <d f>16 <d c' a'>16 bes'16

	fis16 <fis, fis>16 

	<c,, d, c a'>8 <bes a'>8 

	<a c d' c''>8

	a8 bes16 gis16 <g  c'>16 <d, c a'>16 <bes d>8 cis8 

	<c f>16 <g d>16 <d d'>8. b'16 

	<a fis>8 g8 dis16 dis,16 <dis, a b'>8 <a fis> <g dis'> <dis' fis'> a'16

	b'16 a'16 <fis'>16 <g dis'>8 <d, b a'>8	e32 f32

	d16 <c a'>16 <bes fis>16 f16 <f c'>8

	<d c' a''>8 

	<bes, aes aes' c''>4~
	<bes, aes aes' c''>2~
	<bes, aes aes' c''>4
	r4
}

\score { 
        \new PianoStaff  {  \mus }
        \layout { } 
        \midi { } 
} 


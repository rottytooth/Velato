\version "2.16.0"  % necessary for upgrading to future LilyPond versions.

\header{
  title = "Hello, World"
  subtitle = "A program for Velato"
}

mus = { 
	\time 4/4
	<f d' c'' a'''>4		% Print 'H'
	bes8 cis8

	<g c' d''>4				% Print 'e' (starting with d) 
	<c a'>4
 
	<ges' bes'>8. 
	f'16
	ges'16
	<c' f'>8		% Root note twice
	<f g'>16 				% Change to key of C (starting with g)
	<c a'>8  				% Print 'l'
	<g, e f' cis'>4

	<g' a c,>8

	\tuplet 3/2 {a16 g16 e'16} 
	\tuplet 3/2 {f''16 cis''16 c''16} % Print 'l'
	

	<a g' c''>4 
	<d f>4 				% Change to key of F
	<a' c' d>16 bes'16 		% Print 'o'

	fis16 <fis, fis>16

	<c,, d, c a'>8 <bes a'>8 % Print ',' (starting with d)

	<a c d' c''>4.			% Print ' ' (starting with d)

	\tuplet 3/2 {a8 <bes aes>8 g16 c'16}
	
	<d, c a'>16 <bes d>8 cis8 % Print "W"

	<c f>16 <d g>16 % Change key to D
	
	<d d'>8. % reiterating root
	
	b'16 % Print 'o'

	<a fis>8 g8 dis16
	dis,16 <dis, a b'>8 % Print "r" (starting with final b)
	
	<a fis> <dis' g> <dis' fis'> a'16

	b'16 a'16 <fis'>16 <g dis'>8
	
	
	<d, b a'>8.	% Print "l"
	
	% measure
	
	\time 2/4
	
	e32 f32 % Change key to F

	d16 <c a'>16 <bes fis>16 f16 <f c'>8 	% Print "d"

	<d c' a''>8 % Print "!"

	% measure

	\time 4/4
	
	<bes, aes aes' c''>1
}

\score { 
        \new PianoStaff  {  \mus }
        \layout { } 
        \midi { } 
} 


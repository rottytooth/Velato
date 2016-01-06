\version "2.16.0"  % necessary for upgrading to future LilyPond versions.

\header{
  title = "Hello, World"
  subtitle = "A test program for Velato"
}


\parallelMusic #'(voiceA voiceB) {
\time 2/4

  % Bar 1
  <c'' a'''>4 r4 |
   <f d'>4 bes8 cis8	|

  % Bar 2
	<c' d''>4 a'8 bes'8  |
	g4 c4  	  |

  % Bar 3
	fis'8 f'16 fis'16 
	<c' f'>8
	g'16 a'16 	|
	r4.
	f16 c16		|

  % Bar 4
	<f' cis'>4

	<a g'>4 |
	
	<g, e>4
	c,4 |
	
  % Bar 5
	
	<e' f''>4 
	cis''8 
	c''8	|

	
	<a, g>4
	r4	|
	
  % Bar 6
	\time 9/16
	<g' c''>8 r16 <c' a'>16 
	
	bes'16

	r16 r16 

	<d a'>8 |

	a8 <d f>16 d8
	fis16 <fis, fis>16 
	<c,, c,>8 |
	
  % Bar 7
  \time 2/4
	a'8 

	<d' c''>8

	r4 			|
	bes8 <a c>8
	a8 bes16 gis16		|
	
  % Bar 8

	c'16 a'16 
	bes8
	r4
	
		|
	g16 <d, c>16
	
	d8 cis8 

	<c f>16 <g d>16  |
	
  % Bar 9

	 d'8. b'16 

	r4  |
	
	d8 r8
	<a fis>8 g8	|
	
  % Bar 10
  
   dis8  <a b'>8 r4	|
   r16 dis,16 dis,8 <a fis>4 |
	
 % Bar 11
 	
 <dis'> <dis' fis'>	|
 
 g2 |
 
 % Bar 12
 
 a'16 b'16 a'16 <fis'>16 
 dis'8 <b a'>8	|
 
 r4 
 g8 d,8	|

 % Bar 13
	\time 9/16
 
r8 a'16 r8 r8 a''8

 |
 
 e32 f32 d16 c16
 <bes fis>16 
 
 f16
 
 <f c'>8  <d c'>8

 |
 
  % Bar 14
  \time 2/4
	<aes' c''>2~ |

	<bes, aes>2~
	|
  
  % Bar 15
	<aes' c''>2 |

	<bes, aes>2
	|
 
 }



\score { 
        \new StaffGroup <<
		  \new Staff << \voiceA >>
		  \new Staff { \clef bass \voiceB }
		 >>
        \layout { } 
        \midi { \tempo 4 = 90 } 
} 


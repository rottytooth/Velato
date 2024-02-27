\version "2.24.3"
%\language "english"
\score {
\layout{}
{
  d e f f g d
  d bes a f % declare A3 as char - basic notes are C3 octave
  d f a % let A3 =
  f g g gis a % char 45 is a "-"
  d bes e' e %declare e4 as int 
  f e' f a c a % let e4 = 8
  
  d b a f e a % print A3
  d b a f e e' % print e4
  
  %fails to compile with this code
  d bes fis e %declare f#3 as int
  f fis f a c a % let f#3 = 8
  d b a f e fis % print f#3
  
  % will compile with this code
  %d bes f e %declare g#3 as int
  %f f f a c a % let g#3 = 8
  %d b a f e f % print g#3
  
  

}

\midi{}
}

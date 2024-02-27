\version "2.24.3"
melody = \relative c' {
  \clef treble
  \key c \major
  \time 4/4

  c				% set root
  a g			% print
	e f			% a character 
	a dis		% value of H
	g			% end of char marker
  c				% this does nothing the second time but can be used between commands
  a g			% print
	e f			% a character 
	d cis fis	% value of i
	g			% end of char marker
  c				% an extra c because why not

}

\score {
  \new Staff \melody
  \midi { }
}

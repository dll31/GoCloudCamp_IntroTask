#include <list>
#include <functional>

#include "track.h"


typedef std::list<std::reference_wrapper<Track>>::iterator lrefiterator;

class Playlist
{
	std::list<std::reference_wrapper<Track>> playlist;
    long id;

public:
	std::string name;

	Playlist() {}

	lrefiterator getNext(lrefiterator current);
	lrefiterator getPrew(lrefiterator current);

	void addTrack(Track& tr);
	void deleteTrack(Track& tr);
	void deleteTrack(lrefiterator it);

    long getId();

	lrefiterator getPlaylistBegin();

};


void Playlist::addTrack(Track& tr)
{
	playlist.push_back(tr);
}


lrefiterator Playlist::getPlaylistBegin()
{
	return playlist.begin();
}
#include <string>
#include <list>
#include <iterator>

#include "playlist.h"


lrefiterator Playlist::getNext(lrefiterator current)
{
	return ++current;
}


lrefiterator Playlist::getPrew(lrefiterator current)
{
	return --current;
}


void Playlist::deleteTrack(Track& tr)
{
	playlist.remove(std::ref(tr));
}


void Playlist::deleteTrack(lrefiterator it)
{
	playlist.erase(it);
}


long Playlist::getId()
{
	return id;
}



// управляющая сущность 

/* подумать
(возможно нужно создать поток, который будет постоянно крутиться в виде сервера и принимать запросы от апи)
(или проще это сделать из апи, управлять всеми сущностями)
*/
// загружает треки из базы
// создает плеер

class TBD
{


};

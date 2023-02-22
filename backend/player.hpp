#include <thread>
#include <iostream>

#include "playlist.h"
#include "playerEmulator.h"


enum PlayerErrors
{
	OK = 0
    // TODO: TBD
};


/*enum PlayerEmulatorErrors
{
	OK = 0
	// TODO: TBD
};
*/


class Player
{
	Playlist& refplaylist;
	lrefiterator current;

	PlayerEmulator playerEmulator;
	boost::signals2::signal<void (int)> emulatorSignal;

public:

	Player(Playlist& pl) : refplaylist {pl} 
	{
		current = refplaylist.getPlaylistBegin();
		playerEmulator.duration = (*current).get().getDuration();

		emulatorSignal.connect(boost::bind(&PlayerEmulator::emulatorSlot, boost::ref(playerEmulator)));

	}

	//Track& getCurrentSong();
	
	PlayerErrors play();
	PlayerErrors pause();
	PlayerErrors next();
	PlayerErrors prew();

};


PlayerErrors Player::play()
{
    //std::cout << "play" << std::endl;
    emulatorSignal(SignalOperationCode::play);
	return OK;
}

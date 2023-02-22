#include <iostream>

#include "player.hpp"

#define BOOST_THREAD_USE_LIB


int main()
{
    Track t {"track1", 20};

    Playlist pl;
    pl.addTrack(t);

    Player player = Player(pl);    

    player.play();

    return 0;
}

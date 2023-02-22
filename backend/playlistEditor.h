#include "track.h"


class PlaylistEditor
{
public:	
	// возможно создание плейлиста

	// добавляет ссылку на трек из глобального пространства в плейлист
	void AddSong(Track& tr);
	// просто удаляет трек из плейлиста
	void DeleteSong();

};
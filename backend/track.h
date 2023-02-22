#include <string>


class Track
{
	//long id;
	std::string name;
	int duration;
	std::string path;

public:
    Track(std::string name, int duration) : name {name}, duration {duration} {}

	//long getId();
	std::string getName();
	int getDuration();
};


std::string Track::getName()
{
    return name;
}


int Track::getDuration()
{
    return duration;
}

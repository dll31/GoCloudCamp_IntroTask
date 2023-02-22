#include <chrono>

#include <boost/asio/io_service.hpp>
#include <boost/bind/bind.hpp>
#include <boost/signals2/signal.hpp>
#include <boost/thread.hpp>


enum SignalOperationCode
{
	play = 1,
	pause = 2,
	next = 3,
	prew = 4
};


enum PlayerEmulatorState
{
    created = 0,
    played = 1,
    paused = 2,
    stoped = 3
};


class PlayerEmulator
{
public:
	int duration;

    void emulatorSlot();
    void start();

private:
	boost::asio::io_service emulator;

	void init(int duration);

    void signalHandler(SignalOperationCode signalCode);

    void loop();

    boost::thread m_thread;
    PlayerEmulatorState m_thread_state = created;
};

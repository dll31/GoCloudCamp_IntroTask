#include "playerEmulator.h"

#include <iostream>


void PlayerEmulator::init(int duration)
{
    this->duration = duration;
}


void PlayerEmulator::signalHandler(SignalOperationCode signalCode) 
{
    std::cout << "signalHandler" << std::endl;

    switch (signalCode)
    {
    case SignalOperationCode::play:
        std::cout << "handle signal: play" << std::endl;
        if ((m_thread_state != created) && (m_thread_state != played)) start();
        break;

    case SignalOperationCode::next: case SignalOperationCode::prew:
        break;

    case SignalOperationCode::pause:
        
        break;

    default:
        break;
    }
}


void PlayerEmulator::start()
{
    std::cout << "start" << std::endl;
    
    m_thread_state = played;
    m_thread = boost::thread(boost::bind(&PlayerEmulator::loop, this));

}


void PlayerEmulator::emulatorSlot()
{
    emulator.post(boost::bind(&PlayerEmulator::signalHandler, this));
    std::cout << "emulatorSlot" << std::endl;
}


void PlayerEmulator::loop()
{
    emulator.run_for(std::chrono::duration<int, std::chrono::seconds> {duration}); // processes the tasks
}
#ifndef OUT_STREAM_MANAGER_FSS_2014_02_17
#define OUT_STREAM_MANAGER_FSS_2014_02_17

#include <iostream>

using std::ostream;

class COstreamManager
{
public:
    virtual ~COstreamManager(){};
    virtual ostream& GetCurrOutStream(ulong ulLogInfoLen) = 0;
};

#endif

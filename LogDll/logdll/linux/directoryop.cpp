#include <sys/stat.h>

#include "directoryop.h"

bool CreateDirectory(const char* name, void*)
{
	return(mkdir(name, 777) !=0 );
}
#pragma warning ( disable : 4996)

#include <stdarg.h>
#include <stdio.h>
#include <iostream>
#include <set>

#include "logapi.h"

#include "logapp.h"
#include "loginfo.h"
#include "buildintype.h"
#include "logexception.h"

using std::cerr;
using std::set;

set<CLogApp*> g_pLogSet;

void CheckLogAppExist(const string& strLogPath);
void CheckLogMax();

const int MAX_LOGAPP_NUM = 5;

bool LogInitialize(unsigned long* ulLogHandle, const char *pLogPath, unsigned short usMaxFileSize, unsigned char ucMaxFileCount)
{
    CLogApp* pLogApp = NULL;

    try
    {
        CheckLogMax();

        CheckLogAppExist(pLogPath);

        pLogApp = new CLogApp();

        pLogApp->Initailize(pLogPath, usMaxFileSize, ucMaxFileCount);

        g_pLogSet.insert(pLogApp);

        *ulLogHandle = (unsigned long) pLogApp;

        return true;
    }
    catch(exception& excp)
    {
        if (pLogApp != NULL)
        {
            g_pLogSet.erase(pLogApp);

            pLogApp->Exit();

            delete pLogApp;
            pLogApp = NULL;
        }

        cerr<<"Log Initialize failed, reason: "<<excp.what();
        return false;
    }
}

void WriteLogInfo(unsigned long ulLogHandle, unsigned char ucLogLevel, char* pLogInfo)
{
    CLogApp* pLogApp = (CLogApp* ) ulLogHandle;
    if (g_pLogSet.find(pLogApp) == g_pLogSet.end())
    {
        return;
    }

    //char pLogBuff[MAX_LOGINF_LEN];

    //va_list argList;
    //va_start(argList, pLogFormat);

    //(void)vsnprintf(pLogBuff, MAX_LOGINF_LEN, pLogFormat, argList);
    //pLogBuff[MAX_LOGINF_LEN-1] = '\0';

    //va_end(argList); 

    pLogApp->WriteLog(ucLogLevel, pLogInfo);
}

void LogExit(unsigned long ulLogHandle)
{
    CLogApp* pLogApp = (CLogApp* ) ulLogHandle;
    if (g_pLogSet.find(pLogApp) == g_pLogSet.end())
    {
        return;
    }

    g_pLogSet.erase(pLogApp);   

    pLogApp->Exit();

    delete pLogApp;
    pLogApp = NULL;
}

bool SetLogLevel(unsigned long ulLogHandle, uchar ucLogLevel)
{
    CLogApp* pLogApp = (CLogApp* ) ulLogHandle;
    if (g_pLogSet.find(pLogApp) == g_pLogSet.end())
    {
        return false;
    }

    return pLogApp->SetLogLevel(ucLogLevel);
}

void CheckLogMax()
{
    if (g_pLogSet.size() > MAX_LOGAPP_NUM)
    {
        throw CLogException("too many log init!");
    }
}

void CheckLogAppExist(const string& strLogPath)
{
    set<CLogApp*> ::iterator iter = g_pLogSet.begin();

    for (; iter!=g_pLogSet.end(); iter++)
    {
        if(strLogPath == (*iter)->GetLogPath())
        {
            throw CLogException("already have same log path!");
        }
    }
}
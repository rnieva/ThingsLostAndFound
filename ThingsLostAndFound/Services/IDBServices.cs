﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Services
{
    public interface IDBServices
    {
        //ControlPanelController
        Setting GetSettings();
        void ChangeSettings(Setting settings);
        int FoundObjectsFOTotal();
        int FoundObjectsFOsolved();
        int FoundObjectsFOpending();
        int LostObjectsLOTotal();
        int LostObjectsLOsolved();
        int LostObjectsLOpending();
        int MessagesTotal();
        int FilesTotal();
        int UsersTotal();
        //FileController
        File getFile(int id);
        //FindMatchesController
        List<LostObject> getMatchesInLO(FoundObject foundObject);
        List<FoundObject> getMatchesInFO(LostObject lostObject);


    }
}
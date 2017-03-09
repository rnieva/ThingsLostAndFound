using System;
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
        //FoundObjectsController
        List<FoundObject> GetListFO();
        FoundObject GetDetailsFO(int? id);
        void AddFileFO(File file);
        void AddFO(FoundObject foundObject);
        void AddMessage(Message msg);
        void SaveChanges();
        InfoUser GetInfoUser(int? UserIdreported);
        void ModifiedFile(File file);
        void ModifiedFO(FoundObject foundObject);
        InfoUser GetInfoUserByNameContact(string nameContact);
        void AddLostAndFoundObjects(LostAndFoundObject lostAndFoundObject);
        List<Message> MsgListAbouthisFoundObject(int id);
        //InfoUserController
        void AddInfoUser(InfoUser infoUser);
        void ModifiedInfoUser(InfoUser infoUser);
        //LoginController
        string CheckNewMessage(int id);
        //LostObjectController
        List<LostObject> GetListLO();
        LostObject GetDetailsLO(int? id);
        void AddFileLO(File file);
        void AddLO(LostObject lostObject);
        void ModifiedLO(LostObject lostObject);
        //MessagesController
        List<Message> MsgListAbouthisLostObject(int id);
        List<Message> GetListMessages(int id);
        List<Message> GetListNewMessages(int id);
        Message GetMessage(int id);
        //ShowObjectUsersController
        List<FoundObject> GetListFOByUser(int id);
        List<LostObject> GetListLOByUser(int id);
    }
}
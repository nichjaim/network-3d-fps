
[System.Serializable]
public enum HavenLocation
{
    None,

    //---Non-Activity Locations---

    // First-Floor
    FrontHallway1F,
    BackHallway1F,
    RightHallway1F,
    LeftHallway1F,
    FrontRightCorner1F,
    FrontLeftCorner1F,
    BackRightCorner1F,
    BackLeftCorner1F,
    CentralCourtyardOuter,
    // Second-Floor
    FrontHallway2F,
    BackHallway2F,
    RightHallway2F,
    LeftHallway2F,

    //---Activity Locations---

    // First-Floor
    MainGate,
    Library,
    ArtClubrooms,
    FacilityRooms,
    Cafeteria,
    Gym,
    Pool,
    Field,
    CourtyardInner,
    // Secomd-Floor
    FrontRooftop,
    BackRooftop,
    GameClubrooms,
    TeacherLounge

    // Other-Ideas

    //NurseOffice,
    //MCRoom,
    //LockerRoom,
    //StorageRoom,
    //OldBuilding
}

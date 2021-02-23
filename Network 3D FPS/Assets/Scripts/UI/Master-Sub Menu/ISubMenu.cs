
public interface ISubMenu
{
    /// <summary>
    /// Turns on or off the menu's object
    /// </summary>
    /// <param name="isActiveArg"></param>
    void SetMenuObjectActive(bool isActiveArg);

    /// <summary>
    /// Refreshes the menu content to a more current state.
    /// </summary>
    void RefreshSubMenu();

    /// <summary>
    /// Sets reference to sub menu's master.
    /// </summary>
    void SetMenuMaster(MasterMenuController menuMasterArg);
}

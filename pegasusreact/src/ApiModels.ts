export interface MenuItemApiModel {
    Title: string;
    Url: string;
    Active: boolean;
    SubMenuItems: MenuItemApiModel[];
}

export interface MenuApiModel {
    MenuItems: MenuItemApiModel[];
}

export interface PageApiModel {
    MainMenu: MenuApiModel;
    SideMenu: MenuApiModel;
    PageTitle: string;
    Title: string;
    Content: string;
}

﻿using Abp.Application.Navigation;
using Abp.Localization;
using HomeRoom.Authorization;

namespace HomeRoom.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See Views/Layout/_TopMenu.cshtml file to know how to render menu.
    /// </summary>
    public class HomeRoomNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "Home",
                        L("HomePage"),
                        url: "/",
                        icon: "fa fa-home",
                        requiresAuthentication: true
                        )
                ).AddItem(
                new MenuItemDefinition(
                    "Users",
                    L("Users"),
                    url: "/Users",
                    icon: "fa fa-user"
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        "Classes",
                        L("Classes"),
                        url: "/Class",
                        icon: "fa fa-university",
                        requiresAuthentication: true
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        "About",
                        L("About"),
                        url: "/About",
                        icon: "fa fa-info"
                        )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, HomeRoomConsts.LocalizationSourceName);
        }
    }
}

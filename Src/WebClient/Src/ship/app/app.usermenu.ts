import { MainMenuItem } from '../../infrastructure/services/mainmenu.service';

export let userMenu: Array<MainMenuItem> = [
    {
        text: '',
        image: '../assets/icons/dualog-user.png',
        route: null,
        submenu: [
            {
                text: 'Profile',
                icon: '',
                route: '/',
                submenu: null
            },
            {
                text: 'Settings',
                icon: '',
                route: '/',
                submenu: null
            },
            {
                text: 'Help',
                icon: '',
                route: '/',
                submenu: null
            },
            {
                text: 'Sign out',
                icon: '',
                route: '/logout',
                submenu: null
            }
        ],
    }
];

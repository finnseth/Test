import { MainMenuItem } from '../../infrastructure/services/mainmenu.service';

export let mainMenu: Array<MainMenuItem> = [
    {
        text: 'Information',
        icon: 'fa-area-chart',
        route: null
    },
    {
        text: 'Configuration',
        icon: 'fa-cogs',
        route: null,
        submenu: [
            {
                text: 'Organization',
                icon: '',
                route: null,
                submenu: [
                    {
                        text: 'Ship',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'User group',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'User',
                        icon: '',
                        route: null,
                        submenu: null
                    }
                ]
            }
        ],
    },
    {
        text: 'Task',
        icon: 'fa-tasks',
        route: null
    }
];

import { MainMenuItem } from './../../infrastructure/services/mainmenu.service';
import { AccessRights } from './../../infrastructure/domain/permission/permission';

export let mainMenu: Array<MainMenuItem> = [
    {
        text: 'Information',
        icon: 'fa-area-chart',
        route: '/information',
        access: AccessRights.None,
        submenu: [
            {
                text: 'Dashboard',
                icon: '',
                route: '/information/dashboard',
                submenu: null,
                access: AccessRights.None
            },
            {
                text: 'Reports',
                icon: '',
                route: null,
                access: AccessRights.None,
                submenu: [
                    {
                        text: 'Connections',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Internet',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Messages',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Quota',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    }
                ]
            }
        ]
    },
    {
        text: 'Configuration',
        icon: 'fa-cogs',
        route: '/configuration',
        access: AccessRights.None,
        submenu: [
            {
                text: 'Organization',
                icon: 'dualog-organization-icon-16',
                gridImage: 'assets/img/organization.png',
                route: '/configuration/organization',
                access: AccessRights.None,
                submenu: [
                    {
                        text: 'Company',
                        gridImage: 'assets/img/fleet.png',
                        route: '/configuration/organization/company',
                        submenu: null,
                        access: AccessRights.None,
                    },
                    {
                        text: 'Ship',
                        gridImage: 'assets/img/ship.png',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'User group',
                        gridImage: 'assets/img/usergroup.png',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'User',
                        gridImage: 'assets/img/user.png',
                        route: '/configuration/organization/users',
                        submenu: null,
                        access: AccessRights.None
                    }
                ]
            },
            {
                text: 'Email',
                icon: 'dualog-email-icon-16',
                gridImage: 'assets/img/email.png',
                route: '/configuration/email',
                access: AccessRights.None,
                submenu: [
                    {
                        text: 'Setup',
                        icon: '',
                        route: null,
                        access: AccessRights.None,
                        submenu: [
                            {
                                text: 'Domain',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None,
                            },
                            {
                                text: 'Delivery',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None,
                            },
                            {
                                text: 'Technical',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None,
                            }
                        ]
                    },
                    {
                        text: 'Restriction',
                        icon: '',
                        route: '/configuration/email/restriction',
                        access: AccessRights.None,
                        submenu: [
                            {
                                text: 'Quarantine',
                                icon: '',
                                route: '/configuration/email/restriction/quarantine',
                                submenu: null,
                                access: AccessRights.None,
                            },
                            {
                                text: 'Receive filter',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None,
                            },
                            {
                                text: 'User limitation',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None,
                            }
                        ]
                    },
                    {
                        text: 'Address',
                        icon: '',
                        route: null,
                        access: AccessRights.None,
                        submenu: [
                            {
                                text: 'Alias',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            },
                            {
                                text: 'Distribution list',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            }
                        ]
                    },
                    {
                        text: 'Message copying',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Message tracking',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Backup',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    }
                ]
            },
            {
                text: 'File transfer',
                icon: 'dualog-filetransfer-icon-16',
                gridImage: 'assets/img/filetransfer.png',
                route: null,
                access: AccessRights.None,
                submenu: [
                    {
                        text: 'To ship',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'From ship',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Ship to ship',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    }
                ]
            },
            {
                text: 'Antivirus',
                icon: 'dualog-antivirus-icon-16',
                gridImage: 'assets/img/antivirus.png',
                route: null,
                access: AccessRights.None,
                submenu: [
                    {
                        text: 'Setup',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Client management',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    }
                ]
            },
            {
                text: 'Network',
                icon: 'dualog-networkcontrol-icon-16',
                gridImage: 'assets/img/networkcontrol.png',
                route: null,
                access: AccessRights.None,
                submenu: [
                    {
                        text: 'Architecture',
                        icon: '',
                        route: null,
                        access: AccessRights.None,
                        submenu: [
                            {
                                text: 'Internet gateway',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            },
                            {
                                text: 'Internet subnet',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            },
                            {
                                text: 'GPS',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            }
                        ]
                    },
                    {
                        text: 'Failover',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Restriction',
                        icon: '',
                        route: null,
                        access: AccessRights.None,
                        submenu: [
                            {
                                text: 'Bandwidth',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            },
                            {
                                text: 'Quota',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            },
                            {
                                text: 'Proxy',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            },
                            {
                                text: 'Internet rule',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            },
                            {
                                text: 'HTTP rule',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            },
                            {
                                text: 'Content filter',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            },
                            {
                                text: 'DNS filter',
                                icon: '',
                                route: null,
                                submenu: null,
                                access: AccessRights.None
                            }
                        ]
                    },
                    {
                        text: 'Enable service',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Port forwarding',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    },
                    {
                        text: 'Port',
                        icon: '',
                        route: null,
                        submenu: null,
                        access: AccessRights.None
                    }
                ]
            }
        ],
    },
    {
        text: 'Task',
        icon: 'fa-tasks',
        route: null,
        access: AccessRights.None,
        submenu: [
            {
                text: 'Manage crew',
                icon: '',
                route: null,
                access: AccessRights.None,
                submenu: null
            },
            {
                text: 'Message handling',
                icon: '',
                route: null,
                access: AccessRights.None,
                submenu: [
                    {
                        text: 'Release quarantine',
                        icon: '',
                        route: null,
                        access: AccessRights.None,
                        submenu: null
                    },
                    {
                        text: 'Resend messages',
                        icon: '',
                        route: null,
                        access: AccessRights.None,
                        submenu: null
                    }
                ]
            }
        ]
    }
];

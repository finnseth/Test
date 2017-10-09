import { MainMenuItem } from '../../infrastructure/services/mainmenu.service';

export let mainMenu: Array<MainMenuItem> = [
    {
        text: 'Information',
        icon: 'fa-area-chart',
        route: '/information',
        submenu: [
            {
                text: 'Dashboard',
                icon: '',
                route: '/information/dashboard',
                submenu: null
            },
            {
                text: 'Reports',
                icon: '',
                route: null,
                submenu: [
                    {
                        text: 'Connections',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Internet',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Messages',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Quota',
                        icon: '',
                        route: null,
                        submenu: null
                    }
                ]
            }
        ]
    },
    {
        text: 'Configuration',
        icon: 'fa-cogs',
        route: '/configuration',
        submenu: [
            {
                text: 'Organization',
                icon: 'dualog-organization-icon-16',
                gridImage: 'assets/img/organization.png',
                route: '/configuration/organization',
                submenu: [
                    {
                        text: 'Company',
                        gridImage: 'assets/img/fleet.png',
                        route: '/configuration/organization/company',
                        submenu: null
                    },
                    {
                        text: 'Ship',
                        gridImage: 'assets/img/ship.png',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'User group',
                        gridImage: 'assets/img/usergroup.png',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'User',
                        gridImage: 'assets/img/user.png',
                        route: '/configuration/organization/users',
                        submenu: null
                    }
                ]
            },
            {
                text: 'Email',
                icon: 'dualog-email-icon-16',
                gridImage: 'assets/img/email.png',
                route: '/configuration/email',
                submenu: [
                    {
                        text: 'Setup',
                        icon: '',
                        route: null,
                        submenu: [
                            {
                                text: 'Domain',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'Delivery',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'Technical',
                                icon: '',
                                route: null,
                                submenu: null
                            }
                        ]
                    },
                    {
                        text: 'Restriction',
                        icon: '',
                        route: '/configuration/email/restriction',
                        submenu: [
                            {
                                text: 'Quarantine',
                                icon: '',
                                route: '/configuration/email/restriction/quarantine',
                                submenu: null
                            },
                            {
                                text: 'Receive filter',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'User limitation',
                                icon: '',
                                route: null,
                                submenu: null
                            }
                        ]
                    },
                    {
                        text: 'Address',
                        icon: '',
                        route: null,
                        submenu: [
                            {
                                text: 'Alias',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'Distribution list',
                                icon: '',
                                route: null,
                                submenu: null
                            }
                        ]
                    },
                    {
                        text: 'Message copying',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Message tracking',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Backup',
                        icon: '',
                        route: null,
                        submenu: null
                    }
                ]
            },
            {
                text: 'File transfer',
                icon: 'dualog-filetransfer-icon-16',
                gridImage: 'assets/img/filetransfer.png',
                route: null,
                submenu: [
                    {
                        text: 'To ship',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'From ship',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Ship to ship',
                        icon: '',
                        route: null,
                        submenu: null
                    }
                ]
            },
            {
                text: 'Antivirus',
                icon: 'dualog-antivirus-icon-16',
                gridImage: 'assets/img/antivirus.png',
                route: null,
                submenu: [
                    {
                        text: 'Setup',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Client management',
                        icon: '',
                        route: null,
                        submenu: null
                    }
                ]
            },
            {
                text: 'Network',
                icon: 'dualog-networkcontrol-icon-16',
                gridImage: 'assets/img/networkcontrol.png',
                route: null,
                submenu: [
                    {
                        text: 'Architecture',
                        icon: '',
                        route: null,
                        submenu: [
                            {
                                text: 'Internet gateway',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'Internet subnet',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'GPS',
                                icon: '',
                                route: null,
                                submenu: null
                            }
                        ]
                    },
                    {
                        text: 'Failover',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Restriction',
                        icon: '',
                        route: null,
                        submenu: [
                            {
                                text: 'Bandwidth',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'Quota',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'Proxy',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'Internet rule',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'HTTP rule',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'Content filter',
                                icon: '',
                                route: null,
                                submenu: null
                            },
                            {
                                text: 'DNS filter',
                                icon: '',
                                route: null,
                                submenu: null
                            }
                        ]
                    },
                    {
                        text: 'Enable service',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Port forwarding',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Port',
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
        route: null,
        submenu: [
            {
                text: 'Manage crew',
                icon: '',
                route: null,
                submenu: null
            },
            {
                text: 'Message handling',
                icon: '',
                route: null,
                submenu: [
                    {
                        text: 'Release quarantine',
                        icon: '',
                        route: null,
                        submenu: null
                    },
                    {
                        text: 'Resend messages',
                        icon: '',
                        route: null,
                        submenu: null
                    }
                ]
            }
        ]
    }
];

import {Dashboard} from './Dashboard';

    export const DASHBOARDS: Dashboard[] = [
        {
            id: 1,
            title: 'Alert',
            widgets: [
                {
                    title: 'Pie',
                    subtitle: 'Pie show bla bla bla',
                    id: 'pie_chart',
                    data: [
                        ['Task', 'Hours per Day'],
                        ['Work', 11],
                        ['Eat', 2],
                        ['Commute', 2],
                        ['Watch TV', 2],
                        ['Sleep', 7]
                    ],
                    options: {
                        title: 'My Daily Activities'
                    },
                    type: 'PieChart'
            },
                {
                    title: 'Bar',
                    subtitle: 'Bar show bla bla bla',
                    id: 'bar_chart',
                    data: [
                        ['City', '2010 Population', '2000 Population'],
                        ['New York City, NY', 8175000, 8008000],
                        ['Los Angeles, CA', 3792000, 3694000],
                        ['Chicago, IL', 2695000, 2896000],
                        ['Houston, TX', 2099000, 1953000],
                        ['Philadelphia, PA', 1526000, 1517000]
                    ],
                    options: {
                        title: 'Population of Largest U.S. Cities',
                        hAxis: {
                            title: 'Total Population',
                            minValue: 0
                        },
                        vAxis: {
                            title: 'City'
                        }
                    },
                    type: 'BarChart'
                }
            ]
        },
        {
            id: 2,
            title: 'Status',
            widgets: [
                {
                    title: 'Pie2',
                    subtitle: 'Pie2 show bla bla bla',
                    id: 'pie_chart2',
                    data: [
                        ['Task2', 'Hours per Day'],
                        ['Work2', 1],
                        ['Eat2', 22],
                        ['Commute2', 22],
                        ['Watch TV2', 22],
                        ['Sleep2', 27]
                    ],
                    options: {
                        title: 'My 2 Daily Activities'
                    },
                    type: 'PieChart'
                }
            ]
        }
    ];

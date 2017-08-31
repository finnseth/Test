export class Dashboard {
  id: number;
  title: string;
  widgets: Widget[];
}

export class Widget {
    title: string;
    subtitle: string;
    id: string;
    data: any;
    options: WidgetOptions;
    type: string;
}

export class WidgetOptions {
    title: string;
    hAxis?: WidgetOptionsHAxis;
    vAxis?: WidgetOptionsVAxis;
}

export class WidgetOptionsHAxis {
    title: string;
    minValue?: number;
}

export class WidgetOptionsVAxis {
    title: string;
}

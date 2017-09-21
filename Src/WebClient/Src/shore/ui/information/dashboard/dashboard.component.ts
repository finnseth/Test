import { } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { Observable } from 'rxjs/Rx';

import { date } from 'ng2-validation/dist/date';

import { DashboardService, IDashboardInformation, IWidgetColumns, IWidgetInformation, IWidgetResult } from './dashboard.service';
import { Dashboard } from './dashboard';



@Component({
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css'],
    providers: [ DashboardService ]
})

export class DashboardComponent implements OnInit {

    // All available retrieved dashboards
    dashboards: IDashboardInformation[];

    //  Current dashboards retrieved widgets
    widgets: IWidgetInformation[] = [];

    selectedDashboard: IDashboardInformation;

    constructor(private dashboardService: DashboardService) {
    }

    ngOnInit() {

        // Get available dashboards
        this.getDashboards();
    }

    showWidgets(dashboard: IDashboardInformation): void {

        this.selectedDashboard = dashboard;
        this.getWidgetsByDashboardId(dashboard.id);
    }

    /**
     * Get all available dashboards
     */
    private getDashboards(): void {
        const subscription = this.dashboardService.GetDashboards().subscribe( dashboards => {
            this.dashboards = dashboards;
            if (this.dashboards.length > 0) {
                this.showWidgets(this.dashboards[0]);
            }
            subscription.unsubscribe();
        });
    }

    /**
     * Get all widgets related to a given dashboard
     */
    private getWidgetsByDashboardId( id: number): void {

        const subscription = this.dashboardService.GetWidgetsByDashboardId(id).subscribe(widgets => {
            this.widgets = widgets;
            for (const widget of widgets) {

                this.getWidgetById(widget.id);

                subscription.unsubscribe();
            }
        },  );
    }

    /**
     * Get information for on widget based on it's id
     */
    private getWidgetById(id: number): void {

        const subscription = this.dashboardService.GetWidgetData(id).subscribe(result => {

            const widget = this.getWidgetIndexById(id);

            switch (widget.widgetType) {
                case 'Pie': {
                    widget.data = this.processPieData(result);
                    break;
                }
                case 'Bar': {
                    widget.data = this.processBarData(result);
                    break;
                }
                case 'Table': {
                    widget.data = this.processTableData(result);
                    widget.columns = this.processTableDataColumns(result);
                    break;
                }
            }

            subscription.unsubscribe();
        });
    }

    private processPieData(result: IWidgetResult): any {

            const labels: string[] = [];
            const datasets: any[] = [{
                        backgroundColor: [],
                        borderColor: [],
                        data: []
                    }];
            for (const row of result.data) {
                labels.push(row.field);
                datasets[0].backgroundColor.push(this.getRandomColor());
                datasets[0].borderColor.push(this.getRandomColor());
                datasets[0].data.push(row.value);
            }

            return {
                labels: labels,
                datasets: datasets
            };
    }

    private processBarData(result: IWidgetResult): any {

            const labels: string[] = [];
            const datasets: any[] = [];
            for (const row of result.data) {
                labels.push(row.field);
                datasets.push(
                    {
                        label: row.field,
                        backgroundColor: this.getRandomColor(),
                        borderColor: this.getRandomColor(),
                        data: [row.value]
                    }
                );
            }

            return {
                labels: labels,
                datasets: datasets
            };
    }

     private processTableData( data: IWidgetResult): any {

           const result: any[] = [];

           data.data.forEach(c => {



           });


            return result;
    }

    private processTableDataColumns(result: IWidgetResult): any {

            const columns: any[] = [];

            for (const row of result.columns) {
                columns.push({
                    field: row.name,
                    header: row.name
                });
            }

            return columns;
    }

    private getRandomColor(): string{
        return '#' + Math.floor(Math.random() * 16777215).toString(16);
    }

    private getWidgetIndexById( id: number ): IWidgetInformation {

        for (const index in this.widgets) {
            if (this.widgets.hasOwnProperty(index)) {
                if (this.widgets[index].id === id) {
                    return this.widgets[index];
                }
            }
        }
        throw new Error('No Widget found by this id: ' + id.toString());
    }

    /**
     * @param id The id of the widget to edit
     */
    private editWidget(id: number): void {

        alert(id);
    }
}

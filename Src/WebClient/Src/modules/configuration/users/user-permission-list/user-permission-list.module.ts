import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserPermissionListComponent } from './user-permission-list.component';
import { ButtonModule,
         RadioButtonModule,
         DataTableModule,
         ToolbarModule,
         SelectButtonModule,
         DropdownModule,
         DialogModule,
         SplitButtonModule } from 'primeng/primeng';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RadioButtonModule,
        DataTableModule,
        ButtonModule,
        ToolbarModule,
        SelectButtonModule,
        DropdownModule,
        SplitButtonModule,
        DialogModule
    ],

    declarations: [
        UserPermissionListComponent
    ],

    exports: [
        UserPermissionListComponent
    ]
})
export class UserPermissionListModule {}

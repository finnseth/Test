import { MenubarModule } from 'primeng/primeng';
import { HorizontalSubMenuComponent } from './hSubMenu.component';
import {NgModule} from '@angular/core';

@NgModule({
    imports: [
        MenubarModule
    ],

    exports: [
        HorizontalSubMenuComponent
    ],

    declarations: [
        HorizontalSubMenuComponent
    ]
})
export class HorizontalSubMenuModule { }

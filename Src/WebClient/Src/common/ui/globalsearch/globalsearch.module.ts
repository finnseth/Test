import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GlobalSearchComponent } from './globalsearch.component';
import { SearchboxModule } from '../searchbox/searchbox.module';

@NgModule({
    imports: [
        CommonModule,
        SearchboxModule
    ],
    declarations: [
        GlobalSearchComponent
    ],
    exports: [
        GlobalSearchComponent
    ]})
export class GlobalSearchModule {}

import { Component, HostListener, OnInit } from '@angular/core';

import { ComponentCanDeactivate } from '../../connection-suite-shore/services/pending_changes.service';
import { Observable } from 'rxjs/Rx';

export abstract class DualogController implements ComponentCanDeactivate {

    @HostListener('window:beforeunload',['$event'])
    canDeactivate(): Observable<boolean> | boolean {
        // insert logic to check if there are pending changes here;
        // returning true will navigate without confirmation
        // returning false will show a confirm dialog before navigating away
        return !this.pendingChanges();
    }

    public abstract pendingChanges(): boolean;

}

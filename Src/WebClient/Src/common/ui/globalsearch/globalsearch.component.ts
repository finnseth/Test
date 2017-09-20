import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { SearchResult } from 'common/services/search/searchresult';
import { SearchService } from 'common/services/search/search.service';
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'dua-globalsearch',
  templateUrl: './globalsearch.component.html',
  styleUrls: ['./globalsearch.component.scss']
})
export class GlobalSearchComponent implements OnInit {
  results: SearchResult[] = [];
  query = new Subject<string>();
  popupTop = 0;
  @ViewChild('searchbox', {read: ElementRef}) searchBox: ElementRef;

  constructor(private searchService: SearchService) {
    this.searchService.searchChanged.subscribe(change => {
      this.results.length = 0; // New search initiated
    });

    this.searchService.search(this.query)
      .subscribe(result => {
        if (result && result.elements && result.elements.length > 0) {
          this.results.push(result);
        }
      },
    error => {
      const i = 0;
    });
  }

  ngOnInit() {
  }

  ngAfterContentInit () {
    this.popupTop = this.searchBox.nativeElement.offsetTop + this.searchBox.nativeElement.offsetHeight + 4;
  }
}

import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import { SearchService } from '../../search/search.service';
import { SearchResult } from '../../search/searchresult';

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

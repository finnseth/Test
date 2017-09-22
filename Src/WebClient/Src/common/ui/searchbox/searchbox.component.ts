import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import {
    SearchResult,
    SearchResultElement
} from 'common/services/search/searchresult';

import { Observable } from 'rxjs/Observable';
import { SearchService } from 'common/services/search/search.service';
import { Subject } from 'rxjs/Subject';

@Component({
    selector: 'dua-searchbox',
    templateUrl: './searchbox.component.html',
    styleUrls: ['./searchbox.component.scss']
})
export class SearchboxComponent implements OnInit {
    results: SearchResult[] = [];
    query = new Subject<string>();
    popupTop = 0;
    searchValue = '';
    selectedElement: SearchResultElement;
    selectedCategory: SearchResult;
    @ViewChild('searchbox', { read: ElementRef })
    searchBox: ElementRef;

    constructor(private searchService: SearchService) {
        this.searchService.searchChanged.subscribe(change => {
            this.results.length = 0; // New search initiated
            this.selectedCategory = null;
            this.selectedElement = null;
        });

        this.searchService.search(this.query).subscribe(
            result => {
                if (this.results.length === 0) {
                    this.popupTop =
                        this.searchBox.nativeElement.offsetTop +
                        this.searchBox.nativeElement.offsetHeight + 1;
                }
                if (result && result.elements && result.elements.length > 0) {
                    this.results.push(result);
                    this.selectDefaultCategoryAndElement();
                }
            },
            error => {
                console.log('Search error');
                console.log(error);
            }
        );
    }

    ngOnInit() {}

    onKeydown(event) {
        this.selectDefaultCategoryAndElement();
        if (this.selectedCategory == null) {
            return;
        }

        switch (event.which) {
            case 40: {
                // Down
                const currentElementIndex = this.getElementIndex(
                    this.selectedCategory,
                    this.selectedElement
                );
                if (
                    currentElementIndex <
                    this.selectedCategory.elements.length - 1
                ) {
                    // Move down within current category
                    this.selectedElement = this.selectedCategory.elements[
                        currentElementIndex + 1
                    ];
                } else {
                    // Current category exhausted, move to next category or jump to top
                    const currentCategoryIndex = this.getCategoryIndex(
                        this.selectedCategory
                    );
                    if (currentCategoryIndex < this.results.length - 1) {
                        this.selectedCategory = this.results[
                            currentCategoryIndex + 1
                        ];
                    } else {
                        this.selectedCategory = this.results[0];
                    }
                    this.selectedElement = this.selectedCategory.elements[0];
                }
                event.preventDefault();
                break;
            }

            case 38: {
                // Up
                const currentElementIndex = this.getElementIndex(
                    this.selectedCategory,
                    this.selectedElement
                );
                if (currentElementIndex > 0) {
                    this.selectedElement = this.selectedCategory.elements[
                        currentElementIndex - 1
                    ];
                } else {
                    const currentCategoryIndex = this.getCategoryIndex(
                        this.selectedCategory
                    );
                    if (currentCategoryIndex > 0) {
                        this.selectedCategory = this.results[
                            currentCategoryIndex - 1
                        ];
                    } else {
                        this.selectedCategory = this.results[
                            this.results.length - 1
                        ];
                    }
                    this.selectedElement = this.selectedCategory.elements[
                        this.selectedCategory.elements.length - 1
                    ];
                }

                event.preventDefault();
                break;
            }

            case 13:
                this.selectItem(this.selectedCategory, this.selectedElement);
                event.preventDefault();
                break;

            case 27:
                this.clearSearch();
                event.preventDefault();
                break;
        }
    }

    clearSearch() {
        this.results.length = 0;
        this.searchValue = '';
    }

    selectItem(category: SearchResult, element: SearchResultElement) {
        console.log(category);
        console.log(element);
        this.clearSearch();
        alert(element.name + ' => ' + element.route);
        // todo: navigate to element.route
    }

    getCategoryIndex(category: SearchResult): number {
        let index = -1;
        if (category != null) {
            for (let i = 0; i < this.results.length; i++) {
                if (category === this.results[i]) {
                    index = i;
                    break;
                }
            }
        }
        return index;
    }

    getElementIndex(
        category: SearchResult,
        element: SearchResultElement
    ): number {
        let index = -1;
        if (category != null && element != null) {
            for (let i = 0; i < category.elements.length; i++) {
                if (element === category.elements[i]) {
                    index = i;
                    break;
                }
            }
        }
        return index;
    }

    selectDefaultCategoryAndElement(): void {
        if (this.selectedCategory == null) {
            if (this.results.length > 0) {
                this.selectedCategory = this.results[0];
                if (
                    this.selectedCategory.elements &&
                    this.selectedCategory.elements.length > 0
                ) {
                    this.selectedElement = this.selectedCategory.elements[0];
                } else {
                    this.selectedElement = null;
                }
            }
        }
    }
}

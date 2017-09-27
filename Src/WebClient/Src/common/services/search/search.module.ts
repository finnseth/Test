import { NgModule } from '@angular/core';
import { SearchService } from './search.service';
import { ShipSearchProvider } from '../../domain/ship/shipsearchprovider';
import { CompanySearchProvider } from '../../domain/company/companysearchprovider';
import { UserSearchProvider } from '../../domain/user/usersearchprovider';



@NgModule({
     providers: [
         SearchService,
         ShipSearchProvider,
         CompanySearchProvider,
         UserSearchProvider],
})
export class SearchModule { }

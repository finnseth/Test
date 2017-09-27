import { Ship } from './../../../common/domain/ship/interfaces';


export interface ShoreShip extends Ship {
    quarantineLocalChanges?: boolean;
}

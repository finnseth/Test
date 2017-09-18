export { DualogCommonModule } from './dualog-common.module';

export { ConfigurationReader, IConfiguration } from './services/configuration-reader.service';
export { ApiService, PaginationInfo } from './services/api-base.service';
export { JsonSchema }  from './schema/json-schema';
export {SchemaFormBuilder }  from './schema/schema-form-builder';

export interface JsonPatchOperation {
    op: 'test' | 'remove' | 'add' | 'replace' | 'move';
    path: string;
    value?: any;
    from?: string;
}

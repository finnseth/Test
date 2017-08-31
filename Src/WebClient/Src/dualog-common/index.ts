export { DualogCommonModule } from './dualog-common.module';

export { ConfigurationReader, IConfiguration } from './services/configuration-reader.service';
export { ApiService } from './services/api-base.service';
export { JsonSchema }  from './services/json-schema';
export {SchemaFormBuilder }  from './services/schema-form-builder.service';

export interface JsonPatchOperation {
    op: 'test' | 'remove' | 'add' | 'replace' | 'move';
    path: string;
    value?: any;
    from?: string;
}

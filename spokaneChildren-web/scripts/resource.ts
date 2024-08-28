export enum ResourceCategory {
  Therapy,
  Psychiatrist,
  Other,
}

export interface Resource {
  resourceId: number;
  category: ResourceCategory;
  name: string;
  website: string | null;
  phone: string | null;
  address: string | null;
}

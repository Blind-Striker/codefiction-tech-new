import { IRelation } from './IRelation';
import { IPerson } from './IPerson';

export interface IPodcast {
  id: number;
  season: number;
  title: string;
  slug: string;
  soundcloudId: string;
  youtubeUrl: string;
  shortDescription: string;
  longDescription: string;
  guest: IPerson;
  attendees: IPerson[];
  tags: string[];
  relations: IRelation[];
  publishDate: Date;
}

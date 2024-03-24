import { createElement } from 'react';

//https://github.com/kolodny/react-compose-wrappers/blob/master/index.ts
export const nestComponents = (wrappers: React.FunctionComponent[]): React.FunctionComponent => {
    return wrappers.reduceRight((Acc: any, Current: any): React.FunctionComponent => {
      return props => createElement(
        Current,
        null,
        createElement(Acc, props as any)
      );
    });
  }

export default nestComponents;

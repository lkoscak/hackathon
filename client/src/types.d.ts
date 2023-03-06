declare module "*.png";
declare module "*.svg";
declare module "*.jpeg";
declare module "*.jpg";
declare module 'react-paho-mqtt'

declare global {
    interface Window {
        Paho: any
    }
}
export {};
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IRI.Jab.Common.Assets.ShapeStrings
{
   public static class SegoePrint
    {
        // Exterior Ring
        public const string exteriorRing = "M18.64,17.39l-1.8.09a15.51,15.51,0,0,0-1.63.17,2.3,2.3,0,0,0-.73.18.77.77,0,0,0-.08.36,9.68,9.68,0,0,1-.21,1,5.41,5.41,0,0,0-.18.92c.25,0,.61,0,1.08-.08a8.18,8.18,0,0,0,1-.15,6.34,6.34,0,0,1,1.3-.2c.62,0,.93.18.93.55a.6.6,0,0,1-.26.56,2.12,2.12,0,0,1-.81.21l-.76.11-.56.07A13,13,0,0,0,14,21.5h0a.29.29,0,0,0-.15.14,7.45,7.45,0,0,0-.18.87,8,8,0,0,0-.12,1.07.23.23,0,0,0,.27.21c.18,0,.68-.05,1.47-.15s1.3-.18,1.49-.22a7.29,7.29,0,0,1,1.05-.17c.5.07.74.3.74.7a.52.52,0,0,1-.17.41,4.87,4.87,0,0,1-1.63.32c-.53,0-1.18.1-1.94.2a8.91,8.91,0,0,1-1.92.22,1,1,0,0,1-.69-.2,1,1,0,0,1-.25-.63l0-.2a1.85,1.85,0,0,1,.1-.35,23.37,23.37,0,0,0,.58-3.21c.22-1.65.34-2.58.34-2.76,0,0,0,0,0-.05a1,1,0,0,1-.14-.23.86.86,0,0,1-.09-.39.44.44,0,0,1,0-.17c0-.08.1-.16.25-.24a.94.94,0,0,1,.43-.12l.56,0c.32,0,1.37-.12,3.16-.35l.93-.12h.08c.71-.06,1.06.17,1.06.68a.54.54,0,0,1-.19.34A.81.81,0,0,1,18.64,17.39Zm2.8.28-.08-.14a1,1,0,0,1-.19-.43.6.6,0,0,1,.29-.49A.92.92,0,0,1,22,16.4l.27,0,.2,0a18.34,18.34,0,0,1,2.43-.27,3.16,3.16,0,0,1,2.06.62,1.88,1.88,0,0,1,.76,1.53A2.34,2.34,0,0,1,27,20a5.44,5.44,0,0,1-2,1.28,28.08,28.08,0,0,0,3.17,2.32.68.68,0,0,1,.38.59.67.67,0,0,1-.27.47.8.8,0,0,1-.5.23,1.45,1.45,0,0,1-.77-.29q-.43-.3-1.23-.93c-.52-.42-.83-.67-.91-.75a15.4,15.4,0,0,0-1.61-1.23.55.55,0,0,1-.62-.62.61.61,0,0,1,.28-.5,1,1,0,0,1,.62-.2l.22,0,.16,0a3.54,3.54,0,0,0,1.39-.63,1.75,1.75,0,0,0,1-1.44.85.85,0,0,0-.42-.71,1.93,1.93,0,0,0-1.08-.29,7.1,7.1,0,0,0-1.26.09,2.21,2.21,0,0,0-.52.14.42.42,0,0,0-.11.18c0,.1-.12.5-.26,1.21s-.25,1.47-.36,2.32a15.65,15.65,0,0,0-.16,1.86,3.51,3.51,0,0,0,.06.68,3.74,3.74,0,0,1,.05.53.65.65,0,0,1-.25.45.72.72,0,0,1-.54.25c-.54,0-.81-.38-.81-1.13a21.23,21.23,0,0,1,.2-2.56,27.56,27.56,0,0,1,.48-2.89c0-.14.05-.26.07-.36Z";

        public static readonly Geometry erGeometry = Geometry.Parse(exteriorRing);

        //Envelope
        public const string envelope = "M16.07,9.21l-1.81.09a15,15,0,0,0-1.62.17,2.81,2.81,0,0,0-.74.18.82.82,0,0,0-.07.36,7.83,7.83,0,0,1-.22,1,6.38,6.38,0,0,0-.17.92c.25,0,.61,0,1.08-.08s.8-.1,1-.14a5.91,5.91,0,0,1,1.31-.21c.62,0,.92.18.92.55a.6.6,0,0,1-.25.56,2,2,0,0,1-.81.21l-.76.11-.56.07a15.44,15.44,0,0,0-1.92.29h0a.23.23,0,0,0-.15.14,7.45,7.45,0,0,0-.18.87A7,7,0,0,0,11,15.4a.23.23,0,0,0,.27.21c.19,0,.68,0,1.48-.15s1.29-.18,1.49-.21a5.84,5.84,0,0,1,1-.17q.75.11.75.69a.5.5,0,0,1-.18.41,5.09,5.09,0,0,1-1.62.33c-.54,0-1.18.09-2,.19a8.91,8.91,0,0,1-1.92.22,1,1,0,0,1-.68-.2,1,1,0,0,1-.25-.63l0-.19a1.28,1.28,0,0,1,.1-.36,25.78,25.78,0,0,0,.57-3.21c.23-1.65.34-2.57.34-2.76l0-.05a.74.74,0,0,1-.14-.22.81.81,0,0,1-.1-.39.4.4,0,0,1,0-.17c0-.09.09-.17.25-.25a.9.9,0,0,1,.42-.12,5.25,5.25,0,0,1,.56,0c.33,0,1.38-.12,3.17-.35l.93-.12h.08c.7,0,1.05.17,1.05.68a.58.58,0,0,1-.19.35A.84.84,0,0,1,16.07,9.21Zm6.27,6.41a6.1,6.1,0,0,1-.41-2q-.12-1.5-.57-1.5c-.28,0-.77.52-1.46,1.56A11.06,11.06,0,0,0,18.62,16a.77.77,0,0,1-.19.37.4.4,0,0,1-.29.14c-.59,0-.89-.27-.89-.81a9.44,9.44,0,0,1,.18-1.25,18.1,18.1,0,0,0,.23-3.1,1,1,0,0,1,.24-.59.65.65,0,0,1,.51-.31.76.76,0,0,1,.52.17q.18.16.18.9a12.65,12.65,0,0,1-.34,2.16c.85-2,1.8-3,2.84-3s1.44.71,1.59,2.14.34,2.18.54,2.18a.94.94,0,0,0,.31-.06,1.07,1.07,0,0,1,.34-.07c.19,0,.28.11.28.33a.92.92,0,0,1-.39.64,1.44,1.44,0,0,1-1,.33,1,1,0,0,1-1-.59Z";

        public static readonly Geometry enGeometry = Geometry.Parse(envelope);

        //Convex Hull
        public const string convexHull = "M16.67,22.88c.3,0,.45.14.45.43s-.43.8-1.29,1.33a4.8,4.8,0,0,1-2.5.79,3.13,3.13,0,0,1-2-.7,2.42,2.42,0,0,1-.89-2,6.29,6.29,0,0,1,.93-3A8.54,8.54,0,0,1,13.73,17a4.46,4.46,0,0,1,2.7-1,1.72,1.72,0,0,1,1.26.46,1.52,1.52,0,0,1,.46,1.1,3.31,3.31,0,0,1-.1,1,1,1,0,0,1-.45.5,1.18,1.18,0,0,1-.56.21.56.56,0,0,1-.39-.15.58.58,0,0,1-.17-.43,2.59,2.59,0,0,1,.1-.55,2.19,2.19,0,0,0,.1-.6c0-.26-.09-.4-.28-.4a3.07,3.07,0,0,0-1.89.9,8.21,8.21,0,0,0-1.8,2.24A5.09,5.09,0,0,0,12,22.57a1.76,1.76,0,0,0,.39,1.15,1.41,1.41,0,0,0,1.15.47A3.47,3.47,0,0,0,14.63,24a4.82,4.82,0,0,0,1.25-.66A2,2,0,0,1,16.67,22.88Zm4.27-1.26a6.14,6.14,0,0,0-.22,1.75,4.58,4.58,0,0,1-.24,1.4c-.13.37-.31.56-.53.56a.83.83,0,0,1-.86-.9,4.34,4.34,0,0,1,.17-1.07,56.78,56.78,0,0,0,.84-6.43.65.65,0,0,1,.24-.38.62.62,0,0,1,.43-.19q.78,0,.78,1a8.31,8.31,0,0,1-.18,1.38,9.08,9.08,0,0,0-.2,1.58s.17.06.42.06c.55,0,1.32,0,2.32-.13s1.55-.19,1.63-.33a5.33,5.33,0,0,0,.24-1.19,17.18,17.18,0,0,0,.12-2.09.59.59,0,0,1,.25-.39.76.76,0,0,1,1.06,0,.69.69,0,0,1,.21.51,10.79,10.79,0,0,1-.21,1.6,33.23,33.23,0,0,0-.42,3.32c-.11,1.35-.16,2.25-.16,2.69a.9.9,0,0,1-.31.63.81.81,0,0,1-.54.31.78.78,0,0,1-.5-.19.62.62,0,0,1-.24-.5c0-.13.05-.59.16-1.37a15.69,15.69,0,0,0,.16-1.59c0-.19,0-.28-.11-.28s-.55,0-1.09.06-.93.07-1.12.07-.53,0-1.14.07A4.45,4.45,0,0,0,20.94,21.62Z";

        public static readonly Geometry convexHullGeometry = Geometry.Parse(convexHull);

        //Boundary
        public const string boundary = "M17.47,17.58a2.65,2.65,0,0,1,1.42.93,2.1,2.1,0,0,1,.32,1.06,3.2,3.2,0,0,1-1.43,2.3,4.67,4.67,0,0,1-3,1.18c-.59,0-.88-.18-.88-.53,0-.59.4-.88,1.22-.88h.29A2.63,2.63,0,0,0,16.85,21a1.79,1.79,0,0,0,.87-1.35,1,1,0,0,0-.36-.74,1.4,1.4,0,0,0-.95-.32,3.1,3.1,0,0,0-.47.18,1.77,1.77,0,0,1-.62.18,1.32,1.32,0,0,1-.72-.13.71.71,0,0,1-.16-.55c0-.19.4-.45,1.19-.77A4.13,4.13,0,0,0,17,16.54a1.72,1.72,0,0,0,.68-1.1c0-.39-.28-.58-.84-.58a6.64,6.64,0,0,0-1.55.18c-.51.11-.79.26-.85.43s-.2,1.07-.43,2.68-.35,2.55-.35,2.8l-.06.79a1.05,1.05,0,0,1-.28.77.8.8,0,0,1-.52.18q-.78,0-.78-1a2.62,2.62,0,0,1,.1-.56c.17-.71.33-1.7.5-3s.24-1.94.24-2,0-.29-.12-.29h-.16a.38.38,0,0,1-.31-.11.64.64,0,0,1-.17-.23.59.59,0,0,1,0-.21c0-.39.54-.75,1.63-1.1a11,11,0,0,1,3.42-.52,2.32,2.32,0,0,1,1.55.51,1.5,1.5,0,0,1,.59,1.17,1.58,1.58,0,0,1-.38,1A7.68,7.68,0,0,1,17.47,17.58ZM26,19.38a4.73,4.73,0,0,1-1.5,2.31,3.36,3.36,0,0,1-2.19.94A1.81,1.81,0,0,1,20.9,22a2.28,2.28,0,0,1-.56-1.56,5.07,5.07,0,0,1,1-3A3,3,0,0,1,23.87,16a2,2,0,0,1,1.62.74,2.65,2.65,0,0,1,.63,1.77,4.22,4.22,0,0,1-.1.87ZM23,17.53a3.68,3.68,0,0,0-.8,1.16,3.48,3.48,0,0,0-.4,1.6,1.61,1.61,0,0,0,.18.8.53.53,0,0,0,.47.31,2.15,2.15,0,0,0,1.52-.86,2.8,2.8,0,0,0,.78-1.93,1.77,1.77,0,0,0-.25-1,.81.81,0,0,0-.72-.39,1.18,1.18,0,0,0-.78.3Z";

        public static readonly Geometry boundaryGeometry = Geometry.Parse(boundary);


        //Break into geometries
        public const string extractGeometries = "M18,30.36l-1.8.1a15.77,15.77,0,0,0-1.63.16,2.54,2.54,0,0,0-.73.18.66.66,0,0,0-.08.36,9.2,9.2,0,0,1-.21,1,5.41,5.41,0,0,0-.18.92c.25,0,.61,0,1.08-.08s.8-.1,1-.14a5.76,5.76,0,0,1,1.3-.21c.62,0,.93.19.93.55a.6.6,0,0,1-.26.56,1.93,1.93,0,0,1-.81.22c-.23,0-.48.05-.76.1l-.56.07a14.77,14.77,0,0,0-1.91.29h0a.23.23,0,0,0-.15.14,7.45,7.45,0,0,0-.18.87,8.77,8.77,0,0,0-.13,1.07.24.24,0,0,0,.27.22c.19,0,.68-.06,1.48-.16s1.3-.17,1.49-.21a5.77,5.77,0,0,1,1.05-.17c.49.07.74.3.74.69a.52.52,0,0,1-.17.41,5.16,5.16,0,0,1-1.63.33c-.53,0-1.18.09-1.94.2a9.54,9.54,0,0,1-1.92.21,1,1,0,0,1-.69-.2,1,1,0,0,1-.25-.63l0-.19a2.06,2.06,0,0,1,.1-.36A22.35,22.35,0,0,0,12,33.48c.23-1.65.34-2.57.34-2.76l0,0a1,1,0,0,1-.14-.22.94.94,0,0,1-.1-.39.4.4,0,0,1,0-.17c0-.09.1-.17.25-.25a.9.9,0,0,1,.43-.12,5.07,5.07,0,0,1,.55,0c.33,0,1.38-.11,3.17-.35l.93-.12h.08c.71,0,1.06.17,1.06.68a.64.64,0,0,1-.19.35A.78.78,0,0,1,18,30.36Zm8.71,4.37h0A10.15,10.15,0,0,0,26.44,36a7.2,7.2,0,0,0-.14,1.18c0,.45,0,.76,0,.93a.87.87,0,0,1-.11.42.72.72,0,0,1-.26.26.69.69,0,0,1-.36.09.93.93,0,0,1-.58-.22.68.68,0,0,1-.27-.53c0-.06.06-.35.17-.85s.17-.84.23-1.16A6.17,6.17,0,0,1,21.6,37.6,2.3,2.3,0,0,1,19.93,37a2.25,2.25,0,0,1-.62-1.67,5.82,5.82,0,0,1,1-3.22h0a8.59,8.59,0,0,1,2.53-2.53,5.43,5.43,0,0,1,2.93-.93,2.16,2.16,0,0,1,1.19.28.82.82,0,0,1,.44.7,1.45,1.45,0,0,1-.3.8c-.2.28-.4.43-.61.43a.48.48,0,0,1-.29-.08,1,1,0,0,1-.19-.31,1,1,0,0,0-.25-.38.73.73,0,0,0-.42-.1,3.54,3.54,0,0,0-2,.87,7.51,7.51,0,0,0-1.86,2.08,4.53,4.53,0,0,0-.71,2.28c0,.77.33,1.15,1,1.15A3.52,3.52,0,0,0,23,36.11a7,7,0,0,0,1.51-.83,2.45,2.45,0,0,0,1-1,5.08,5.08,0,0,0,.24-1,.58.58,0,0,1,.25-.44,1.1,1.1,0,0,1,.53-.11c.51,0,.77.18.77.52a2.06,2.06,0,0,1-.2.67A5.53,5.53,0,0,0,26.71,34.73Z";

        public static readonly Geometry extractGeometriesGeometry = Geometry.Parse(extractGeometries);


        //Extract points
        public const string extractPoints= "M20.91,17.51l-1.8.09a15,15,0,0,0-1.62.17,2.48,2.48,0,0,0-.74.17.85.85,0,0,0-.08.36,9.24,9.24,0,0,1-.21,1,5.41,5.41,0,0,0-.18.92c.25,0,.61,0,1.08-.08a8.18,8.18,0,0,0,1-.15,6.32,6.32,0,0,1,1.31-.2c.61,0,.92.18.92.55a.61.61,0,0,1-.26.56,2.12,2.12,0,0,1-.81.21l-.76.11-.55.07a13.31,13.31,0,0,0-1.92.29h0a.29.29,0,0,0-.15.14,6.39,6.39,0,0,0-.17.87,7,7,0,0,0-.13,1.07.23.23,0,0,0,.27.21c.18,0,.68-.05,1.48-.15s1.29-.18,1.48-.22a7.29,7.29,0,0,1,1.05-.17c.5.07.74.3.74.69a.48.48,0,0,1-.17.41,4.59,4.59,0,0,1-1.62.33c-.54,0-1.19.1-1.95.2a8.91,8.91,0,0,1-1.92.22,1,1,0,0,1-.69-.2,1,1,0,0,1-.25-.63l0-.2a1.85,1.85,0,0,1,.1-.35,23.37,23.37,0,0,0,.58-3.21c.22-1.66.34-2.58.34-2.76,0,0,0,0,0-.05a1.24,1.24,0,0,1-.14-.23A.86.86,0,0,1,15,17.2.79.79,0,0,1,15,17c0-.08.1-.16.25-.24a1,1,0,0,1,.43-.13,5.46,5.46,0,0,1,.56,0c.32,0,1.38-.12,3.16-.35l.93-.12h.09c.7,0,1,.18,1,.68a.56.56,0,0,1-.19.35A.81.81,0,0,1,20.91,17.51Zm3.38,4.61a20.89,20.89,0,0,0-.15,2.18.89.89,0,0,1-.19.57.63.63,0,0,1-.53.25c-.48,0-.72-.27-.72-.83,0-.31.11-1.35.34-3.13.18-1.44.28-2.3.28-2.57a1.52,1.52,0,0,0-.05-.54.72.72,0,0,1-.38-.7c0-.33.53-.61,1.59-.86a12.74,12.74,0,0,1,2.72-.37,3.66,3.66,0,0,1,2.11.55,1.7,1.7,0,0,1,.78,1.47,2.88,2.88,0,0,1-1.44,2.35,9.31,9.31,0,0,1-3.5,1.43C24.65,22,24.36,22.09,24.29,22.12Zm.52-4.4-.35,3.08a9,9,0,0,0,3-1c.85-.48,1.28-1,1.28-1.54s-.52-1-1.55-1A7.29,7.29,0,0,0,24.81,17.72Z";

        public static readonly Geometry extractPointsGeometry = Geometry.Parse(extractPoints);

    }
}
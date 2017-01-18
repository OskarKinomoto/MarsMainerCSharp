/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using OpenTK;
using System;

namespace MarsMiner
{
    class Camera
    {
        private Vector2 m_position = new Vector2(0,0);

        public Vector2 position
        {
            get
            {
                return m_position;
            }
        }

        public void UpdatePosition(Robot r)
        {
            m_position = -r.Center();
            
			if (-m_position.X - Controler.WindowWidth / 2 < Model.MinTileX)
            {
				m_position.X = -(Model.MinTileX + Controler.WindowWidth / 2);
            }
			if (-m_position.X + Controler.WindowWidth / 2 > Model.MaxTileX)
            {
				m_position.X = -(Model.MaxTileX - Controler.WindowWidth / 2);
            }
        }

    }
}

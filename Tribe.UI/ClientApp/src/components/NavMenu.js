import React from 'react';
import { Container, Navbar, NavbarBrand } from 'reactstrap';

import './NavMenu.css';

const NavMenu = () => {
  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <Container>
          <NavbarBrand>Tribe Weather</NavbarBrand>
        </Container>
      </Navbar>
    </header>
  );
}

export default NavMenu;


import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import { Layout, Menu, theme } from 'antd';
import { LineChartOutlined, UnorderedListOutlined } from '@ant-design/icons';
import WeatherChartPage from './components/WeatherChartPage';
import LogsPage from './components/LogsPage';

const { Header, Content, Footer, Sider } = Layout;

const App: React.FC = () => {
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  return (
    <Router>
      <Layout style={{ minHeight: '100vh' }}>
        <Sider collapsible>
          <div className="demo-logo-vertical" style={{ 
            height: '32px',
            margin: '16px',
            background: 'rgba(255, 255, 255, 0.2)',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: 'white',
            fontWeight: 'bold'
          }}>
            Weather App
          </div>
          <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline">
            <Menu.Item key="1" icon={<LineChartOutlined />}>
              <Link to="/weather">Weather Data</Link>
            </Menu.Item>
            <Menu.Item key="2" icon={<UnorderedListOutlined />}>
              <Link to="/logs">Fetch Logs</Link>
            </Menu.Item>
          </Menu>
        </Sider>
        <Layout>
          <Header style={{ padding: 0, background: colorBgContainer }} />
          <Content style={{ margin: '0 16px' }}>
            <div style={{ 
              padding: 24, 
              minHeight: 360, 
              background: colorBgContainer,
              margin: '16px 0'
            }}>
              <Routes>
                <Route path="/" element={<Navigate to="/weather" replace />} />
                <Route path="/weather" element={<WeatherChartPage />} />
                <Route path="/logs" element={<LogsPage />} />
              </Routes>
            </div>
          </Content>
          <Footer style={{ textAlign: 'center' }}>
            Atea test task ©{new Date().getFullYear()}
          </Footer>
        </Layout>
      </Layout>
    </Router>
  );
};

export default App;